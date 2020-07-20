import { CardResponse } from './cardResponse';
import { ReadResult } from './enums/readResult';

export interface ReadCard {
    id: number;
    ownerId: number;
    cardId: number;
    card: CardResponse;
    jsonData: any;
    currentDeck: number;
    readResult: ReadResult;
}
