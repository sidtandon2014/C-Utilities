Connect-AzAccount

$filesystemName = "plants"
$dirname = "source/"

$storageAccount = Get-AzStorageAccount -ResourceGroupName "rgSampleData" -AccountName "sasampledata"
$ctx = $storageAccount.Context

function GetDirectorySize {
    [CmdletBinding()]
    param([String] $directoryPath )
   
   $totalSize = 0 
   $items = Get-AzDataLakeGen2ChildItem -Context $ctx -FileSystem $filesystemName -Path $directoryPath -OutputUserPrincipalName
   foreach ($item in $items)
   {
       #Write-Host $item.Path
       if($item.IsDirectory -eq "True")
       {
            $totalSize = $totalSize + $(GetDirectorySize($item.Path))
       }
       else{
            $totalSize = $totalSize + $item.Length / 1MB
       }
   }
   return $totalSize
}

$size = GetDirectorySize($dirname)
Write-Host  "$($size) MB"