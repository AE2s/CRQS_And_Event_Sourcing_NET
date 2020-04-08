git add -A 
git commit -m "Resolve test"
git merge step2-test1
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step2.txt | Write-Host -f green
Write-Host ""
Write-Host ""
