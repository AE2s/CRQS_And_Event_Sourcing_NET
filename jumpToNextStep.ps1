git add -A
git commit -m "Abort test"
git checkout -b workshop-step5 step5 
git merge step5-test1 *> $null
git checkout --ours . 
git add . 
git commit -m "Merge with test branch" 
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step5.txt | Write-Host -f green
Write-Host ""
Write-Host ""
