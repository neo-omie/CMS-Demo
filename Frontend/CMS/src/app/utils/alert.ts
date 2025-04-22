import Swal from 'sweetalert2'
import { TYPE } from '../components/auth/login/values.constants'
export  class Alert{
   static toast(typeIcon = TYPE.SUCCESS, timerProgressBar: boolean = false, op:string = "") {
     Swal.fire({
       toast: true,
       position: 'top',
       showConfirmButton: false,
       icon: typeIcon,
       timerProgressBar,
       timer: 5000,
       title: op
     })
   }
   static bigToast(title:string, text:string,icon = TYPE.SUCCESS,confirmButtonText:string){
    Swal.fire({
      title: title,
      text: text,
      icon: icon,
      confirmButtonText: confirmButtonText
    });
   }
}