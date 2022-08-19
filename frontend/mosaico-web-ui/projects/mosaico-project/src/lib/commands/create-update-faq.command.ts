export interface CreateUpdateFaqCommand {
    title: string | null;
    content: string | null;
    isHidden: boolean | null;
    order: number | null;
    language: string | null;
} 