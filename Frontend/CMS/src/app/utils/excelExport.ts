import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';

export class ExcelExport {
    static printToExcel(tableID:string, fileName:string) {
        const element = document.getElementById(tableID); // your table id
        const worksheet: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
        const workbook: XLSX.WorkBook = { Sheets: { 'Sheet1': worksheet }, SheetNames: ['Sheet1'] };
        const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
        const blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
        saveAs(blob, fileName);
    }
}