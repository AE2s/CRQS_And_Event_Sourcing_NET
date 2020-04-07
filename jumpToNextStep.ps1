git add -A
git commit -m "Abort test"
git checkout -b workshop-step2 step2 
git merge step2-test1 *> $null
git checkout --ours . 
git add . 
git commit -m "Merge with test branch" 
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step2.txt | Write-Host -f green
Write-Host ""
Write-Host ""
