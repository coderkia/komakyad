import { CardResponse } from './cardResponse';

export interface ReadCard {
    id: number;
    ownerId: number;
    cardId: number;
    card: CardResponse;
    jsonData: any;
    currentDeck: number;
}
