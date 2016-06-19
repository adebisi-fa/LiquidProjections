$script:ilMergeModule = @{}
$script:ilMergeModule.ilMergePath = $null

<# see this aricle: http://www.mattwrock.com/post/2012/02/29/What-you-should-know-about-running-ILMerge-on-Net-45-Beta-assemblies-targeting-Net-40.aspx #>
function Merge-Assemblies {
	Param(
		$files,
		$outputFile,
		$exclude,
		$keyfile,
		$targetPlatform="v4"
	)

	$exclude | out-file ".\exclude.txt"

	$args = @(
		"/internalize:exclude.txt", 
		"/xmldocs",
		"/wildcards",
		"/targetplatform:$targetPlatform",
        "/allowdup",
		"/out:$outputFile") + $files

	if($ilMergeModule.ilMergePath -eq $null)
	{
		write-error "IlMerge Path is not defined. Please set variable `$ilMergeModule.ilMergePath"
	}

	& $ilMergeModule.ilMergePath $args 

	if($LastExitCode -ne 0) {
		write-error "Merge Failed"
	}
	
	remove-item ".\exclude.txt"
}

Export-ModuleMember -Variable "ilMergeModule" -Function "Merge-Assemblies"