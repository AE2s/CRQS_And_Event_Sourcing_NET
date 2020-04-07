git add -A
git commit -m "Abort test"
git checkout -b workshop-step4 step4 
git merge step4-test1 *> $null
git checkout --ours . 
git add . 
git commit -m "Merge with test branch" 
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step4.txt | Write-Host -f green
Write-Host ""
Write-Host ""
