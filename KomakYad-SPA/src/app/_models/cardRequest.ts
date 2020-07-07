export interface CardRequest {
    id: number;
    answer: string;
    question: string;
    example: string;
    extraData?: string;
    collectionId: number;
}
