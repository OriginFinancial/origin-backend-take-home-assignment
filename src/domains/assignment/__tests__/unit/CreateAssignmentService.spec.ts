import 'reflect-metadata';
import FakeInsuranceProvider from '../../providers/InsuranceProvider/fake/FakeInsuranceProvider';
import CreateAssignmentService from '../../services/CreateAssignmentService';

let fakeInsuranceProvider: FakeInsuranceProvider;
let createAssignmentService: CreateAssignmentService;

describe('Create Assignment Service - unit tests', () => {
  beforeEach(() => {
    fakeInsuranceProvider = new FakeInsuranceProvider();
    createAssignmentService = new CreateAssignmentService(
      fakeInsuranceProvider,
    );
  });

  it('should NOT be able to create an auto insurance without cars', async () => {
    const assignment = createAssignmentService.execute({
      age: 31,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assignment.auto).toBe('ineligible');
  });

  it('should NOT be able to create a disability insurance without income or with age over 60 years old', async () => {
    const assignment1 = createAssignmentService.execute({
      age: 31,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    const assignment2 = createAssignmentService.execute({
      age: 61,
      dependents: 0,
      income: 200000,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assignment1.disability).toBe('ineligible');
    expect(assignment2.disability).toBe('ineligible');
  });

  it('should NOT be able to create a home insurance without houses', async () => {
    const assignment = createAssignmentService.execute({
      age: 31,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assignment.home).toBe('ineligible');
  });

  it('should NOT be able to create a life insurance with age over 60 years old', async () => {
    const assignment = createAssignmentService.execute({
      age: 61,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assignment.life).toBe('ineligible');
  });

  it('should be able to create an auto insurance', async () => {
    const assignment = createAssignmentService.execute({
      age: 31,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
      vehicle: {
        year: 2020,
      },
    });

    expect(assignment.auto).not.toBeUndefined();
    expect(assignment.auto).not.toBe('ineligible');
  });

  it('should be able to create a disability insurance', async () => {
    const assignment = createAssignmentService.execute({
      age: 60,
      dependents: 0,
      income: 200000,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assignment.disability).not.toBeUndefined();
    expect(assignment.disability).not.toBe('ineligible');
  });

  it('should be able to create a home insurance', async () => {
    const assignment = createAssignmentService.execute({
      age: 60,
      dependents: 0,
      income: 200000,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
      house: {
        ownership_status: 'mortgaged',
      },
    });

    expect(assignment.home).not.toBeUndefined();
    expect(assignment.home).not.toBe('ineligible');
  });

  it('should be able to create a life insurance', async () => {
    const assignment = createAssignmentService.execute({
      age: 60,
      dependents: 0,
      income: 0,
      marital_status: 'married',
      risk_questions: [1, 1, 1],
    });

    expect(assignment.life).not.toBeUndefined();
    expect(assignment.life).not.toBe('ineligible');
  });
});
