namespace Pal.Web.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //public class GenericColumnsController : Controller
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly SqlCmd _sqlCommand;
    //    private readonly IEmployeesService _test;

    //    public GenericColumnsController(ApplicationDbContext context, SqlCmd sqlCommand, IEmployeesService test)
    //    {
    //        _context = context;
    //        _sqlCommand = sqlCommand;
    //        _test = test;
    //    }

    //    // --------------------------------------------------------------------------------------------
    //    public IActionResult Index()
    //    {
    //        var allTables = _context.Model.GetEntityTypes().Select(t =>  t.ClrType.Name).ToList();
    //        var editableTables = _context.EditableTables.Select(t => t.tableName).ToList();

    //        allTables = allTables.Where(w => !editableTables.Contains(w)).ToList();

    //        SelectList allTablesDDL = new SelectList(allTables);
    //        SelectList editableTablesDDL = new SelectList(editableTables);

    //        ViewBag.allTables = allTablesDDL;
    //        ViewBag.editableTables = editableTablesDDL;
    //        // for test
    //        var employees = _test.getEmployees().Result;
    //        ViewBag.EditableColumns = _sqlCommand.GetEditableColumns("Employee");
    //        return View(employees);
    //    }

    //    public async Task<IActionResult> AddEditableTables(string tableName)
    //    {
    //        await _context.EditableTables.AddAsync(new EditableTable { tableName = tableName });
    //        _context.SaveChanges();
    //        return View();
    //    }
    //    public IActionResult RemoveEditableTables(string tableName)
    //    {
    //        _context.EditableTables.Remove(new EditableTable { tableName = tableName });
    //        _context.SaveChanges();
    //        return View();
    //    }
    //    // --------------------------------------------------------------------------------------------
    //    public IActionResult showColumn(string modelName)
    //    {   
    //        var result = _sqlCommand.GetEditableColumns(modelName);
    //        return Json(result);
    //    }
    //    public async Task<IActionResult> AddColumnAsync(string modelName, string columnName)
    //    {
    //        var result = await _sqlCommand.AddColAsync(columnName, modelName);
    //        return Json(result > 0 ? true : false);
    //    }
    //    public async Task<IActionResult> RemoveColumnAsync(string modelName, string columnName)
    //    {
    //        var result = await _sqlCommand.RemoveColAsync(columnName, modelName);
    //        return Json(result > 0 ? true : false);
    //    }
    //    // --------------------------------------------------------------------------------------------
    //    public IActionResult Update(string modelName, string pkVal, string colName, string colVal)
    //    {
    //        // temp test
    //        modelName = "Employee";
    //        var x = _sqlCommand.UpdateAsync(modelName, colName, colVal, pkVal);
    //        return Json(x);
    //    }
    //    public IActionResult RemoveVals(string pkVal, string colName)
    //    {
    //        var pkCol = "Id";
    //        var tableName = "Employees";
    //        var x = _sqlCommand.Delete(colName, tableName, pkCol, pkVal);
    //        return Json(x);
    //    }
    //    // --------------------------------------------------------------------------------------------
    //    //public IActionResult GetColumns(string tableName)
    //    //{
    //    //    List<string> Columns = _sqlCommand.GetEditableColumns(tableName);
    //    //    return Json(Columns);
    //    //}


    //}

}
