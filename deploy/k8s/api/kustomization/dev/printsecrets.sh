(
	cd secrets/ #can be removed in the pipeline when set work directory
	
	for file in $(find -type f); do 
	
		envVar=$(echo "${file#"./"}" | tr / _);
		
		eval "tmpval=\"\$$envVar\""
		
		if [ -z "${tmpval}" ]
		then
		  echo "Environment Variable '$envVar' was not defined and is required for $file";
		  #TODO: add script to fail the build
		else
			echo "Loading environment '$envVar' to $file";
			echo "${tmpval}" > $file; #set the value
		fi;
			
	done; 
)