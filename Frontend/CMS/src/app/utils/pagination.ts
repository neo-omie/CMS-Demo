export class Pagination{
    
  static pageNumbers:number[] = [1,1,2,3,4,5];
  static maxPage:number = 1;
    static paginator(pageNumber:number,totalRecords:number,pageSize:number){
        this.pageNumbers[0] = pageNumber;
        if(pageNumber == 1){
            this.maxPage = Math.ceil(totalRecords / pageSize);
        }
        let diff = this.maxPage - pageNumber;
        if(diff >= 0 && this.maxPage >= 5){
            if(this.pageNumbers[0] == 1){
                this.pageNumbers = [1, pageNumber, pageNumber+1,pageNumber+2,pageNumber+3,pageNumber+4];
            }
            else if(this.pageNumbers[0] == 2){
                this.pageNumbers = [2,pageNumber-1,pageNumber,pageNumber+1,pageNumber+2,pageNumber+3];
            }
            else if(diff == 0){
                this.pageNumbers = [pageNumber,pageNumber-4,pageNumber-3,pageNumber-2,pageNumber-1,pageNumber];
            }
            else if(diff == 1){
                this.pageNumbers = [pageNumber,pageNumber-3,pageNumber-2,pageNumber-1,pageNumber,pageNumber + 1];
            }
            else{
                this.pageNumbers = [pageNumber,pageNumber-2,pageNumber-1,pageNumber,pageNumber+1,pageNumber+2];
            }
        }
        return {
            "maxPage":this.maxPage,
            "pageNumbers":this.pageNumbers  
        }
    }
}