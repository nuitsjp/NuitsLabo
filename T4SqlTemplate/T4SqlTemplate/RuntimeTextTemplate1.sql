<#@ template language="C#" linePragmas="false" #>
<#@ assembly name="System.Core" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Text" #>
<#@ import namespace="System.Collections.Generic" #>

select
	*
from
	Employee
where
	1 = 1
<# if(1 == 1) {#>
	and Name = @Name
<# } #>