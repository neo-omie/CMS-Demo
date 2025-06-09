import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';

// export class ExcelExport {
//     static printToExcel(tableID:string, fileName:string) {
//         const element = document.getElementById(tableID); // your table id
//         const worksheet: XLSX.WorkSheet = XLSX.utils.table_to_sheet(element);
//         const workbook: XLSX.WorkBook = { Sheets: { 'Sheet1': worksheet }, SheetNames: ['Sheet1'] };
//         const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
//         const blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
//         saveAs(blob, fileName);
//     }
// }

export class ExcelExport {
  static printToExcel(tableID: string, fileName: string, selectedHeaders: string[]): void {
    const table = document.getElementById(tableID) as HTMLTableElement;
    if (!table) {
      console.error('Table not found.');
      return;
    }

    const headerCells = Array.from(table.querySelectorAll('thead tr th'));
    const headerMap: number[] = [];

    // Map header text to column indices
    selectedHeaders.forEach(headerText => {
      const index = headerCells.findIndex(cell => cell.textContent?.trim() === headerText);
      if (index !== -1) {
        headerMap.push(index);
      }
    });

    // Extract filtered data
    const rows = Array.from(table.querySelectorAll('tbody tr'));
    const data: any[][] = [];

    // Add header row
    data.push(selectedHeaders);

    for (const row of rows) {
      const cells = Array.from(row.querySelectorAll('td'));
      const rowData = headerMap.map(index => cells[index]?.textContent?.trim() || '');
      data.push(rowData);
    }

    const worksheet = XLSX.utils.aoa_to_sheet(data);
    const workbook: XLSX.WorkBook = {
      Sheets: { 'Sheet1': worksheet },
      SheetNames: ['Sheet1']
    };

    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelBuffer], { type: 'application/octet-stream' });
    saveAs(blob, fileName);
  }
}