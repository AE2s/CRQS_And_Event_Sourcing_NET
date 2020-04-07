git add -A 
git commit -m "Resolve test"
git merge step5-test1
Write-Host ""
Write-Host ""
Get-Content stepsDoc/step5.txt | Write-Host -f green
Write-Host ""
Write-Host ""
