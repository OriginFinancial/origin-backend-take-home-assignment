import { IInsuranceScore } from '../contracts/IInsuranceProvider';
import {
  isUnder30Years,
  isBetween30and40Years,
  isIncomeAbove200k,
  isVehicleProducedInLast5Years,
} from '../rulesTypes';

const autoRulesScore: IInsuranceScore[] = [
  { name: isUnder30Years, score: -2 },
  { name: isBetween30and40Years, score: -1 },
  { name: isIncomeAbove200k, score: -1 },
  { name: isVehicleProducedInLast5Years, score: 1 },
];

export default autoRulesScore;
