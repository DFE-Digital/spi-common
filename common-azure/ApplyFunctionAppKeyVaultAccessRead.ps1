param(
	[string] $ResourceGroupName,
	[string] $VaultName,
	[string] $ObjectId
)

Import-Module Azure;
Import-Module AzureRM.Resources;

# Set the access policy.
Set-AzureRmKeyVaultAccessPolicy `
	-VaultName $VaultName `
	-ResourceGroupName $ResourceGroupName `
	-ObjectId $objectId `
	-PermissionsToSecrets get,list `
	-PassThru;