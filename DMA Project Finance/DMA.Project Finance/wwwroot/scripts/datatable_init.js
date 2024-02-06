function init_datatable(table, searching) {
    if ($.fn.dataTable.isDataTable(table) == false) {
        $(table).DataTable({
            "searching": searching
        });
    }
}