import { TYPE } from "../components/auth/login/values.constants";
import { Alert } from "./alert";

export class ValidateFile {
    static validateFile(event: Event) : any {
        const input = event.target as HTMLInputElement;
        if (input.files?.length) {
            if (!input.files[0]) {
                Alert.toast(TYPE.WARNING, true, "Please select a file.");
                return null;
            }
    
            const allowedExtensions = ['.pdf', '.doc', '.docx', '.jpg', '.jpeg', '.png'];
            const fileExtension = input.files[0].name.substring(input.files[0].name.lastIndexOf('.')).toLowerCase();
    
            if (!allowedExtensions.includes(fileExtension)) {
                Alert.toast(TYPE.WARNING, true, "Unsupported file format. Allowed formats: .pdf, .doc, .docx, .jpg, .jpeg and .png.");
                return null;
            }
    
            if (input.files[0].size > 25 * 1048576) {
                Alert.toast(TYPE.WARNING, true, "File too large. Max 25MB allowed.");
                return null;
            }

            return input.files[0];
        }
        else{
            Alert.toast(TYPE.WARNING, true, "Please select a file.");
            return null;
        }
    }
}