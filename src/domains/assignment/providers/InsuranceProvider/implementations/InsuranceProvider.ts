import IInsuranceProvider, {
  IInsurance,
  IInsuranceScore,
  IMainInsuranceRules,
  InsurancePlanType,
} from '../contracts/IInsuranceProvider';

class InsuranceProvider implements IInsuranceProvider {
  public getInsurance({
    baseScore,
    rules,
    rulesScore,
  }: IInsurance): InsurancePlanType {
    const totalScore = this.getInsuranceScore(rules, rulesScore);
    const score = baseScore + totalScore;

    return this.getInsurancePlan(score);
  }

  public getInsurancePlan(score: number): InsurancePlanType {
    if (score <= 0) return InsurancePlanType.economic;
    if (score <= 2) return InsurancePlanType.regular;
    if (score >= 3) return InsurancePlanType.responsible;

    return InsurancePlanType.ineligible;
  }

  public getInsuranceScore(
    baseRules: IMainInsuranceRules,
    insuranceScore: IInsuranceScore[],
  ): number {
    const scores: number[] = [];

    insuranceScore.forEach(rule => {
      if (baseRules[rule.name]) scores.push(rule.score);
    });

    return scores.reduce((previous, current) => previous + current, 0);
  }
}

export default InsuranceProvider;
