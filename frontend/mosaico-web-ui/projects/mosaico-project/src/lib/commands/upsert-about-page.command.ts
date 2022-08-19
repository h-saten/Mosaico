export interface UpsertAboutPageCommand {
    language: string | null;
    pageId: string;
    content: string | null;
} 