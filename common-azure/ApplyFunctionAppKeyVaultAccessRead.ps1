param(
	[string] $ResourceGroupName,
	[string] $VaultName,
	[string] @ObjectId
)

# Set the access policy.
Set-AzureRmKeyVaultAccessPolicy `
	-VaultName $VaultName `
	-ResourceGroupName $ResourceGroupName `
	-ObjectId $objectId `
	-PermissionsToSecrets get,list `
	-PassThru;