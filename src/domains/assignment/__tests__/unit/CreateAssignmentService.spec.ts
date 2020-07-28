import 'reflect-metadata';
import FakeInsuranceProvider from '../../providers/InsuranceProvider/fake/FakeInsuranceProvider';
import CreateAssignmentService from '../../services/CreateAssignmentService';

let fakeInsuranceProvider: FakeInsuranceProvider;
let createAssigmentService: CreateAssignmentService;

describe('Create Assignment Service - unit tests', () => {
  beforeEach(() => {
    fakeInsuranceProvider = new FakeInsuranceProvider();
    createAssigmentService = new CreateAssignmentService(fakeInsuranceProvider);
  });

  it('should NOT be able to create an auto insurance without cars', async () => {
    const assigment = createAssigmentService.execute({
      age: 31,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assigment.auto).toBe('ineligible');
  });

  it('should NOT be able to create a disability insurance without income or with age over 60 years old', async () => {
    const assigment1 = createAssigmentService.execute({
      age: 31,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    const assigment2 = createAssigmentService.execute({
      age: 61,
      dependents: 0,
      income: 200000,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assigment1.disability).toBe('ineligible');
    expect(assigment2.disability).toBe('ineligible');
  });

  it('should NOT be able to create a home insurance without houses', async () => {
    const assigment = createAssigmentService.execute({
      age: 31,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assigment.home).toBe('ineligible');
  });

  it('should NOT be able to create a life insurance with age over 60 years old', async () => {
    const assigment = createAssigmentService.execute({
      age: 61,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assigment.life).toBe('ineligible');
  });

  it('should be able to create an auto insurance', async () => {
    const assigment = createAssigmentService.execute({
      age: 31,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
      vehicle: {
        year: 2020,
      },
    });

    expect(assigment.auto).not.toBe('ineligible');
  });

  it('should be able to create a disability insurance', async () => {
    const assigment = createAssigmentService.execute({
      age: 60,
      dependents: 0,
      income: 200000,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assigment.disability).not.toBe('ineligible');
  });

  it('should be able to create a home insurance', async () => {
    const assigment = createAssigmentService.execute({
      age: 60,
      dependents: 0,
      income: 200000,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
      house: {
        ownership_status: 'mortgaged',
      },
    });

    expect(assigment.home).not.toBe('ineligible');
  });

  it('should be able to create a life insurance', async () => {
    const assigment = createAssigmentService.execute({
      age: 60,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assigment.life).not.toBe('ineligible');
  });
});
