export interface IItemListingResponse {
  id: number;
  name: string;
  description: string;
  picture: string;
  price: number;
  instantPrice: number;
  sellerId: number;
  seller: {
    name: string;
    email: string;
  };
  buyerId: number;
  buyer?: {
    name: string;
    email: string;
  };
}
