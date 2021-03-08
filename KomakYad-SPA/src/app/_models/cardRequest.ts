import { CardJsonData } from "./cardJsonData";

export interface CardRequest {
    id: number;
    answer: string;
    question: string;
    example: string;
    jsonData?: CardJsonData;
    collectionId: number;
}
