export interface CardRequest {
    id: number;
    answer: string;
    question: string;
    example: string;
    jsonData?: string;
    collectionId: number;
}
