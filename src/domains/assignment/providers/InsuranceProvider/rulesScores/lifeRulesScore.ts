import { IInsuranceScore } from '../contracts/IInsuranceProvider';
import {
  isUnder30Years,
  isBetween30and40Years,
  isIncomeAbove200k,
  hasDependents,
  isMarried,
} from '../rulesTypes';

const lifeRulesScore: IInsuranceScore[] = [
  { name: isUnder30Years, score: -2 },
  { name: isBetween30and40Years, score: -1 },
  { name: isIncomeAbove200k, score: -1 },
  { name: hasDependents, score: 1 },
  { name: isMarried, score: 1 },
];

export default lifeRulesScore;
