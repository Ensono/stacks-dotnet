
function Invoke-Terraform() {

    [CmdletBinding()]
    param (

        [string]
        # Path to the terraform files
        $path,

        [Parameter(
            ParameterSetName="apply"
        )]
        [switch]
        # Initalise Terraform
        $apply,

        [Parameter(
            ParameterSetName="custom"
        )]
        [switch]
        # Initalise Terraform
        $custom,        

        [Parameter(
            ParameterSetName="init"
        )]
        [switch]
        # Initalise Terraform
        $init,

        [Parameter(
            ParameterSetName="lint"
        )]
        [switch]
        # Initalise Terraform
        $lint,

        [Parameter(
            ParameterSetName="plan"
        )]
        [switch]
        # Initalise Terraform
        $plan,    
        
        [Parameter(
            ParameterSetName="output"
        )]
        [switch]
        # Initalise Terraform
        $output,
        
        [Parameter(
            ParameterSetName="workspace"
        )]
        [switch]
        # Initalise Terraform
        $workspace,        

        [string[]]
        [Alias("backend", "properties")]
        # Arguments to pass to the terraform command
        $arguments

    )

    # Check parameters exist for certain cmds
    if (@("init").Contains($PSCmdlet.ParameterSetName)) {

        # Check that some backend properties have been set
        if ($arguments.Count -eq 0) {
            Write-Error -Message "No properties have been specified for the backend" -ErrorAction Stop
            return
        }
    }

    if (@("plan", "apply").Contains($PSCmdlet.ParameterSetName)) {
        if ([String]::IsNullOrEmpty($path)) {
            Write-Error -Message "Path to the Terraform files or plan file must be supplied" -ErrorAction Stop
            return
        }

        if (!(Test-Path -Path $path)) {
            Write-Error -Message ("Specified path does not exist: {0}" -f $path) -ErrorAction Stop
            return
        }
    }

    # Find the Terraform command to use
    $terraform = Find-Command -Name "terraform"

    # select operation to run based on the cmd
    switch ($PSCmdlet.ParameterSetName) {

        # Apply the infrastructure
        "apply" {
            $command = "{0} apply {1}" -f $terraform, $path
            Invoke-External -Command $command
        }

        # Run custom terraform command that is not supported by the function
        "custom" {
            $command = "{0} {1}" -f $terraform, ($arguments -join " ")
            Invoke-External -Command $command
        }

        # Initialise terraform
        "init" {

            # Iterate around the arguments
            $a = @()
            foreach ($arg in $arguments) {
                $a += "-backend-config=`"{0}`"" -f $arg
            }

            # Build up the command to pass
            $command = "{0} init {1}" -f $terraform, ($a -join (" "))
            Invoke-External -Command $command
        }

        # Perform linting operations for Terrafform
        "lint" {

            $commands = @()

            # Build up the commands that terraform needs to run to perform linting tasks
            $commands += "{0} fmt -diff -check -recursive" -f $terraform
            $commands += "{0} init -backend=false; {0} validate" -f $terraform

            # Exceute the tarrform command
            foreach ($command in $commands) {
                Invoke-External -Command $command
            }
        }

        # Plan the infrastrtcure
        "plan" {

            # Change directory to the specified path
            Push-Location -Path $path

            $command = "{0} plan {1}" -f $terraform, ($arguments -join " ")
            Invoke-External -Command $command

            # Return to the previous path
            Pop-Location
        }

        # Output information from the state
        "output" {

            # Run the command to get the state from terraform
            $command = "{0} output -json" -f $terraform
            Invoke-External -Command $command
        }


        # Create or select the terraform workspace
        "workspace" {

            Write-Information -MessageData ("Attempting to select workspace: {0}" -f $arguments[0])
            $command = "{0} workspace select {1}" -f $terraform, $arguments[0]
            Invoke-External -Command $command

            # if the lastexitcode is 1 then create the workspace
            if ($LASTEXITCODE -eq 1) {
                Write-Information -MessageData "Creating workspace as it does not exist"
                $command = "{0} workspace new {1}" -f $terraform, $arguments[0]
                Invoke-External -Command $command
            }
        }
    }


}