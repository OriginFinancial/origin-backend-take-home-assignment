import { injectable, inject } from 'tsyringe';
import ICreateAssignmentDTO from '../dtos/ICreateAssignmentDTO';
import autoRulesScore from '../providers/InsuranceProvider/rulesScores/autoRulesScore';
import disabilityRulesScore from '../providers/InsuranceProvider/rulesScores/disabilityRulesScore';
import homeRulesScore from '../providers/InsuranceProvider/rulesScores/homeRulesScore';
import lifeRulesScore from '../providers/InsuranceProvider/rulesScores/lifeRulesScore';
import IInsuranceProvider, {
  IMainInsuranceRules,
  InsurancePlanType,
} from '../providers/InsuranceProvider/contracts/IInsuranceProvider';

interface IResponse {
  auto: InsurancePlanType;
  disability: InsurancePlanType;
  home: InsurancePlanType;
  life: InsurancePlanType;
}

@injectable()
class CreateAssignmentService {
  constructor(
    @inject('insuranceProvider')
    private insuranceProvider: IInsuranceProvider,
  ) {}

  public execute(assignmentData: ICreateAssignmentDTO): IResponse {
    const {
      age,
      dependents,
      income,
      marital_status,
      risk_questions,
      house,
      vehicle,
    } = assignmentData;

    const currentYear = new Date().getFullYear();

    const rules: IMainInsuranceRules = {
      hasVehicles: !!vehicle && Object.keys(vehicle).length > 0,
      isVehicleProducedInLast5Years:
        !!vehicle && currentYear - vehicle.year <= 5,
      hasIncome: income > 0,
      hasHouses: !!house && Object.keys(house).length > 0,
      hasDependents: dependents > 0,
      isOver60Years: age > 60,
      isUnder30Years: age < 30,
      isBetween30and40Years: age >= 30 && age <= 40,
      isIncomeAbove200k: income > 200000,
      isHouseMortgaged: house?.ownership_status === 'mortgaged',
      isMarried: marital_status === 'married',
    };

    const baseScore = risk_questions.reduce(
      (previous, current) => previous + current,
      0,
    );

    const autoInsurancePlan = () => {
      if (!rules.hasVehicles) return InsurancePlanType.ineligible;

      return this.insuranceProvider.getInsurance({
        baseScore,
        rules,
        rulesScore: autoRulesScore,
      });
    };

    const disabilityInsurancePlan = () => {
      if (!rules.hasIncome || rules.isOver60Years) {
        return InsurancePlanType.ineligible;
      }

      return this.insuranceProvider.getInsurance({
        baseScore,
        rules,
        rulesScore: disabilityRulesScore,
      });
    };

    const homeInsurancePlan = () => {
      if (!rules.hasHouses) return InsurancePlanType.ineligible;

      return this.insuranceProvider.getInsurance({
        baseScore,
        rules,
        rulesScore: homeRulesScore,
      });
    };

    const lifeInsurancePlan = () => {
      if (rules.isOver60Years) return InsurancePlanType.ineligible;

      return this.insuranceProvider.getInsurance({
        baseScore,
        rules,
        rulesScore: lifeRulesScore,
      });
    };

    return {
      auto: autoInsurancePlan(),
      disability: disabilityInsurancePlan(),
      home: homeInsurancePlan(),
      life: lifeInsurancePlan(),
    };
  }
}

export default CreateAssignmentService;
