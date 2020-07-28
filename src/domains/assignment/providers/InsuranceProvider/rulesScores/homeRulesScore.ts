import { IInsuranceScore } from '../contracts/IInsuranceProvider';
import {
  isUnder30Years,
  isBetween30and40Years,
  isIncomeAbove200k,
  isHouseMortgaged,
} from '../rulesTypes';

const homeRulesScore: IInsuranceScore[] = [
  { name: isUnder30Years, score: -2 },
  { name: isBetween30and40Years, score: -1 },
  { name: isIncomeAbove200k, score: -1 },
  { name: isHouseMortgaged, score: 1 },
];

export default homeRulesScore;
