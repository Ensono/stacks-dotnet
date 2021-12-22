$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path) -replace ".Tests", ""

Write-Host "$here\$sut"

Describe Invoke-TerraformInit {
    It "Given no parameters will state that directory is not set" {
        Mock Write-Error {} -ParameterFilter { -Message "A test erro" }

        "$here\$sut"

        Assert-MockCalled Write-Error -Exactly 1
    }
}