git add -A
git commit -m "Abort test"
git checkout -b workshop-step3 step3 
git merge step3-test1 *> $null
git checkout --ours . 
git add . 
git commit -m "Merge with test branch" 
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step3.txt | Write-Host -f green
Write-Host ""
Write-Host ""
