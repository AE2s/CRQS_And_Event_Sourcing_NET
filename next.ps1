git add -A 
git commit -m "Resolve test"
git merge step4-test1
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step4.txt | Write-Host -f green
Write-Host ""
Write-Host ""
