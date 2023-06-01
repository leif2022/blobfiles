$sourceDirectory = "E:\LRSDATA\DownloadBatchToProcessCloud\Files"


# Get all subdirectories directly under the source directory
$subdirectories = Get-ChildItem -Path $sourceDirectory -Directory

# Loop through each subdirectory
foreach ($subdirectory in $subdirectories) {
    $destinationFolder = Join-Path -Path $subdirectory.FullName -ChildPath (Get-Date -Format "yyyy-MM-dd")
    
    # Create the destination folder if it doesn't exist
    if (!(Test-Path -Path $destinationFolder -PathType Container)) {
        New-Item -Path $destinationFolder -ItemType Directory | Out-Null
    }
    
    # Get all files in the subdirectory
    $files = Get-ChildItem -Path $subdirectory.FullName -File
    
    # Move the files to the destination folder
    foreach ($file in $files) {
        Move-Item -Path $file.FullName -Destination $destinationFolder
    }
}

