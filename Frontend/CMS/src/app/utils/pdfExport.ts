import { jsPDF } from "jspdf";
import { autoTable } from "jspdf-autotable";

// export class PDFExport {
//     static printToPDF(tableID:string, fileName:string) {
//       let doc = new jsPDF();
//       autoTable(doc, {html: tableID})
//       doc.save(fileName);
//     }
// }

export class PDFExport {
  static printToPDF(tableID: string, fileName: string, includeColumns: string[]) {
    const inputTable = document.getElementById(tableID) as HTMLTableElement;

    if (!inputTable) return;

    const headers: string[] = [];
    const rows: string[][] = [];

    const ths = inputTable.querySelectorAll('thead th');
    const allColumns = Array.from(ths).map(th => th.textContent?.trim() || '');

    // Get indexes of columns to include
    const columnIndexes = includeColumns.map(colName => allColumns.indexOf(colName)).filter(index => index !== -1);

    // Set headers
    columnIndexes.forEach(index => {
      headers.push(allColumns[index]);
    });

    // Process rows
    const trs = inputTable.querySelectorAll('tbody tr');
    trs.forEach(tr => {
      const tds = tr.querySelectorAll('td');
      const rowData: string[] = [];
      columnIndexes.forEach(index => {
        rowData.push(tds[index]?.textContent?.trim() || '');
      });
      rows.push(rowData);
    });

    const doc = new jsPDF();
    autoTable(doc, {
      head: [headers],
      body: rows
    });

    doc.save(fileName);
  }
}