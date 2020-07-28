type HouseRequest = {
  ownership_status: 'owned' | 'mortgaged';
};

type VehicleRequest = {
  year: number;
};

export default interface ICreateAssignmentDTO {
  age: number;
  dependents: number;
  income: number;
  marital_status: 'single' | 'married';
  risk_questions: number[];
  house?: HouseRequest;
  vehicle?: VehicleRequest;
}
