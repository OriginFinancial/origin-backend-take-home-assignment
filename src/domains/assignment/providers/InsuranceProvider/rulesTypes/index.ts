import { IMainInsuranceRules } from '../contracts/IInsuranceProvider';

const hasVehicles: keyof IMainInsuranceRules = 'hasVehicles';
const isVehicleProducedInLast5Years: keyof IMainInsuranceRules =
  'isVehicleProducedInLast5Years';
const hasIncome: keyof IMainInsuranceRules = 'hasIncome';
const hasHouses: keyof IMainInsuranceRules = 'hasHouses';
const hasDependents: keyof IMainInsuranceRules = 'hasDependents';
const isIncomeAbove200k: keyof IMainInsuranceRules = 'isIncomeAbove200k';
const isHouseMortgaged: keyof IMainInsuranceRules = 'isHouseMortgaged';
const isMarried: keyof IMainInsuranceRules = 'isMarried';
const isUnder30Years: keyof IMainInsuranceRules = 'isUnder30Years';
const isBetween30and40Years: keyof IMainInsuranceRules =
  'isBetween30and40Years';

export {
  hasVehicles,
  hasIncome,
  hasHouses,
  isUnder30Years,
  isBetween30and40Years,
  isIncomeAbove200k,
  isVehicleProducedInLast5Years,
  isHouseMortgaged,
  hasDependents,
  isMarried,
};
