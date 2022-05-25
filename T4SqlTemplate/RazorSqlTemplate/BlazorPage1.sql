select
    *
from
    Employee
where
    1 = 1
@if (Model.Foo is null)
{
    and Name = @Model.Name
}