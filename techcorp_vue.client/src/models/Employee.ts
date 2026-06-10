import { reactive } from "vue";

export interface Department {
  id: number;
  name: string;

}

export interface Signature {
  id: number;
  signature: string;

}

export interface Position {
  id: number;
  title: string;
}

export interface Filemodel {
  id: number;
  filepath: string;
}

export interface Employee {
  id: number;
  name: string; // ต้องตรงกับ C# property
  CoverPhotoFile?: File;
  positionid?: number | null;
  managerid?: number | null;
  departmentid?: number | null;
  isdeleted?: boolean;
  idfile?: number | null;
  department?: Department | null;
  idfileNavigation?: Filemodel | null;
  position?: Position | null;
  inverseManager?: Employee[]; // ลูก


  signatureid?: number | null;
  signature?: { signature1: string }; 
}

export class EmployeeCreate {
  id = 0;
  name = "";
  managerid: number | null = null;
  departmentid: number | null = null;
  positionid: number | null = null;
  isdeleted = false;
  idfile: number | null = null;
  idfileNavigation?: Filemodel | null;
  CoverPhotoFile?: File;
  profileImage: string | null = null;
  signatureid: number | null = null;
  signature?: string;

  inverseManager: EmployeeCreate[] = [];
}
