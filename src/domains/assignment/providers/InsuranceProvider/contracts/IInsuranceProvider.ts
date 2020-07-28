export interface IMainInsuranceRules {
  hasVehicles: boolean;
  hasIncome: boolean;
  hasHouses: boolean;
  hasDependents: boolean;
  isVehicleProducedInLast5Years: boolean;
  isOver60Years: boolean;
  isUnder30Years: boolean;
  isBetween30and40Years: boolean;
  isIncomeAbove200k: boolean;
  isHouseMortgaged: boolean;
  isMarried: boolean;
}

export interface IInsuranceScore {
  name: keyof IMainInsuranceRules;
  score: number;
}

export enum InsurancePlanType {
  economic = 'economic',
  regular = 'regular',
  responsible = 'responsible',
  ineligible = 'ineligible',
}

export interface IInsurance {
  baseScore: number;
  rules: IMainInsuranceRules;
  rulesScore: IInsuranceScore[];
}

export default interface IInsuranceProvider {
  getInsurance(data: IInsurance): InsurancePlanType;
  getInsurancePlan(score: number): InsurancePlanType;
  getInsuranceScore(
    baseRules: IMainInsuranceRules,
    insuranceScore: IInsuranceScore[],
  ): number;
}
