<#

    .SYNOPSIS
    Script to convert the Asciidoc files into Markdown

    .DESCRIPTION
    This script is deeigned to work in a DOcker container that has pandoc, asciidoctor and PowerShell
    installed. This is so that this script can run and will generate the markdown files using the following
    steps

    Asciidoc (.adoc) -> Dockbook (.xml) -> Markdown (.md)

#>

[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)]
    [string]
    # Directory containing the Asciidoc files
    $docs_dir,

    [Parameter(Mandatory=$true)]
    [string]
    # Output directory for the markdown files
    $output_dir,

    [switch]
    # State of front matter should be skipped or not
    $SkipFrontMatter
)

# Ensure that the $docs_dir exists
$exists = Test-Path -Path $docs_dir
if (!$exists) {
    Write-Error -Message ("Specified document directory does not exist: {0}" -f $docs_dir)
    exit 1
}

# Ensure that the output directory exists, create it
# This is ncessary due to the way in which the directories are created on the filesystem when
# run in Docker. If directories are created in the mounted volume within Docker, they have
# root permissions on the host filesystem, which means the process may not have permissions
# to write to those directories. By doing it here the problem is circumvented.
$exists = Test-Path -Path $output_dir
if (!$exists) {
    New-Item -ItemType Directory -Path $output_dir | Out-Null
}

# Get a list of the files in the docs dir
$list = Get-ChildItem -Path $docs_dir -Recurse -Attributes !Directory -Include *.adoc

# Iterate around the list of files
foreach ($file in $list) {
    # Set the output paths retaining the original folder structure
    $md_file_path = (Split-Path $file) -replace (Resolve-Path -Path $docs_dir), $output_dir
    $xml_file_path = (Split-Path $file) -replace (Resolve-Path -Path $docs_dir), $docs_dir

    Write-Output ("MD Path: {0}" -f $md_file_path)
    $exists = Test-Path -Path $md_file_path
    if (!$exists) {
        Write-Output ("Creating directory: {0}" -f $md_file_path)
        New-Item -ItemType Directory -Path $md_file_path | Out-Null
    }

    # generate the path for the xml file
    $xml_file = [IO.Path]::Combine($xml_file_path, ("{0}.xml" -f [System.IO.Path]::GetFileNameWithoutExtension($file.FullName)))

    # generate the md path
    $md_file = [IO.Path]::Combine($md_file_path, ("{0}.md" -f[System.IO.Path]::GetFileNameWithoutExtension($file.FullName)))

    # determine the attributes for asciidoctor command
    $attributes = @()
    if ($SkipFrontMatter) {
        $attributes += "-a skip-front-matter"
    }

    # Create the XML file using asciidoctor
    Write-Output ("Generating xml file: {0}" -f $xml_file)
    asciidoctor -b docbook -o $xml_file ($attributes -split " ") $file.FullName

    # Create the markdown file
    Write-Output ("Generating markdown file: {0}" -f $md_file)
    pandoc -f docbook -t gfm $xml_file -o $md_file --wrap=preserve

    # If NOT skipping front matter then prepend the header line to the markdown file, which is left out when put into Docbook format
    if (!$SkipFrontMatter) {

        # Get the content of the file and prepend the line
        $content = Get-Content -Path $md_file -Raw
        $content = "---`n" + $content
        Set-Content -Path $md_file -Value $content
    }

}