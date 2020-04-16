param(
	[string] $ResourceGroupName,
	[string] $VaultName,
	[string] $ObjectId
)

try {
	# Set the access policy.
	Set-AzKeyVaultAccessPolicy `
		-VaultName $VaultName `
		-ResourceGroupName $ResourceGroupName `
		-ObjectId $objectId `
		-PermissionsToSecrets get,list `
		-PassThru;
}
catch [Exception]
{
	echo $_.Exception | Format-List -Force;
}