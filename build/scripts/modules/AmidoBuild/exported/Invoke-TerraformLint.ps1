function Invoke-TerraformLint() {
    [CmdletBinding()]
    param (
        [switch]
        # Peform terraform format check
        $Format,

        [switch]
        # Perform Terraform validation checkes
        $Validate
    )

    # Find the path to terraform
    $terraform = Find-Command -Name "terraform"

    # Perform the approproate command(s) based on the switch
    if ($Format) {
        $cmd = "{0} fmt -diff -check -recursive" -f $terraform
        Invoke-External -Command $cmd
    }

    if ($Validate) {
        $cmd = "{0} init -backend=false; {0} validate" -f $terraform
        Invoke-External -Command $cmd
    }

    exit $LASTEXITCODE
}