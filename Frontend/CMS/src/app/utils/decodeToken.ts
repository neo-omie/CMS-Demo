import { jwtDecode } from "jwt-decode";
import { JwtClaims } from "../models/jwt-claims";

export class DecodeToken {
    
    static sub: string | null;
    static email: string | null;
    static ECode: string | null;
    static ERole: string[] | null;
    static decodeJWTToken(token:string) {
        let decodedToken: JwtClaims = new JwtClaims('','','',[]);
        decodedToken = jwtDecode(token);
        DecodeToken.sub = decodedToken.sub;
        DecodeToken.email = decodedToken.email;
        DecodeToken.ECode = decodedToken.ECode;
        DecodeToken.ERole = decodedToken.ERole;
        return decodedToken;
    }
    static clearUserCredentials() {
        DecodeToken.sub = null;
        DecodeToken.email = null;
        DecodeToken.ECode = null;
        DecodeToken.ERole = null;
    }
}