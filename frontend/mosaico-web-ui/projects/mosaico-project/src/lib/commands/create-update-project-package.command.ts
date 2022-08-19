export interface CreateUpdateProjectPackageCommand {
  // logoUrl?: File | null;
  name: string | null;
  tokenAmount: number;
  benefits: string[] | null;
  language: string;
}
