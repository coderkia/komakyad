import { CardResponse } from './cardResponse';
import { ReadResult } from './enums/readResult';
import { ReadCardJsonData } from './readCardJsonData';

export interface ReadCard {
    id: number;
    ownerId: number;
    cardId: number;
    card: CardResponse;
    jsonData: ReadCardJsonData;
    currentDeck: number;
    readResult: ReadResult;
}
