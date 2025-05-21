import { jsPDF } from "jspdf";
import { autoTable } from "jspdf-autotable";

export class PDFExport {
    static printToPDF(tableID:string, fileName:string) {
      let doc = new jsPDF();
      autoTable(doc, {html: tableID})
      doc.save(fileName);
    }
}