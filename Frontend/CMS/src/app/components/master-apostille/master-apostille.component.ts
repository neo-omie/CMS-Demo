import { Component } from '@angular/core';
import { LoaderComponent } from '../loader/loader.component';
import { CommonModule } from '@angular/common';
import { MasterApostille, MasterApostilleDto } from '../../models/master-apostille';
import { MasterApostilleService } from '../../services/master-apostille.service';
import { Router } from '@angular/router';
import { Alert } from '../../utils/alert';
import { TYPE } from '../auth/login/values.constants';
import { Pagination } from '../../utils/pagination';

@Component({
  selector: 'app-master-apostille',
  standalone: true,
  imports: [LoaderComponent,CommonModule],
  templateUrl: './master-apostille.component.html',
  styleUrl: './master-apostille.component.css'
})
export class MasterApostilleComponent {
loading=true;
errorMsg?:string;
apostilles:MasterApostille[]=[];
totalApostilles:number=0;
totalPages:number=0;
currentPage:number=1;
pageSize:number=2;
maxPage:number=1; //used now 
pageNumbers:number[] = []; //used now 


constructor(private apostilleService: MasterApostilleService,private router: Router){}

ngOnInit(): void {
  this.fetchApostille();
}

fetchApostille(){
  this.apostilleService.getApostilles(this.currentPage,this.pageSize)
  .subscribe({
    next:(response:MasterApostille[])=>{
      this.loading=false;
      console.log(response);
      this.apostilles = response;
      this.totalApostilles=this.apostilles.length;
      if(this.apostilles!= undefined && this.totalApostilles>0){
        let result=Pagination.paginator(this.currentPage,this.totalApostilles,this.pageSize);
        this.maxPage = result.maxPage;
        console.log(this.maxPage);
        this.pageNumbers = result.pageNumbers;
      }
    },
    error:(error)=>{
      this.loading = false;
        console.error('Error :(', error);
        this.errorMsg = JSON.stringify((error.message !== undefined)?error.error.title: error.message);
        Alert.toast(TYPE.ERROR,true,this.errorMsg);
    }
  });
}

GetPage(pgNumber:number){
  if(this.maxPage >= pgNumber && pgNumber >= 1){
    this.currentPage=pgNumber;
    this.fetchApostille();
  }
}

getPageNumbers():number[]{
  const pageNumbers=[];
  for(let i=1; i<=this.totalPages;i++){
    pageNumbers.push(i)
  }
  return pageNumbers
}


deleteApostille(apostille:MasterApostille){
  Alert.confirmToast("Are you sure you want to delete this Employee?",
    "You won't be able to revert this!", TYPE.WARNING,
    "Yes, delete it!",
    "Deleted successfully!",
    "Company has been deleted.", TYPE.SUCCESS,() => {
      this.apostilleService.deleteApostille(apostille.valueId).subscribe({
        next:(response:boolean)=>{
          if(response){
            Alert.toast(TYPE.SUCCESS,true,"Deleted successfully");
            this.fetchApostille();
          }
        }
      });
    });
  }
  
addApostille(){
  this.router.navigate(['masters/apostilleMasters/addApostille']);
}

viewApostille(apostille:MasterApostille){
  this.router.navigate(['masters/apostilleMasters/viewApostille', apostille.valueId]);
}

editApostille(apostille:MasterApostille){
  this.router.navigate(['masters/apostilleMasters/viewApostille', apostille.valueId]);
}
}
