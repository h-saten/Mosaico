export interface FileParameters {
  data: File | null;
  fileName: string | null;
  removeImg: boolean;
}

export interface FileResponse {
  data: Blob;
  status: number;
  fileName?: string;
  headers?: { [name: string]: any };
}

export interface FileContentResult {
  fileContents: string; // base64
  contentType: string;
  fileDownloadName: string;
}
