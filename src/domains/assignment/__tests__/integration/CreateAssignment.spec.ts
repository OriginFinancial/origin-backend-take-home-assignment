import app from '@shared/infra/http/app';
import request from 'supertest';

describe('Create Assignment - integration tests', () => {
  it('should NOT be able to create an assignment with invalid request params', async () => {
    const invalidRequest = [
      {},
      {
        age: '',
      },
      {
        age: 'aaa',
      },
      {
        age: 30,
        dependents: '',
      },
      {
        age: 30,
        dependents: 'aaa',
      },
      {
        age: 30,
        dependents: 0,
        income: '',
      },
      {
        age: 30,
        dependents: 0,
        income: 'aaa',
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: '',
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'aaa',
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: '',
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: 'aaa',
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [],
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [''],
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: ['a'],
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1],
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1, 0],
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1, 0, 1],
        house: {
          aaa: '',
        },
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1, 0, 1],
        house: {
          ownership_status: '',
        },
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1, 0, 1],
        house: {
          ownership_status: 'aaaa',
        },
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1, 0, 1],
        house: {
          ownership_status: 'owned',
        },
        vehicle: {
          aaa: '',
        },
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1, 0, 1],
        house: {
          ownership_status: 'owned',
        },
        vehicle: {
          year: '',
        },
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1, 0, 1],
        house: {
          ownership_status: 'owned',
        },
        vehicle: {
          year: 'aaaa',
        },
      },
      {
        age: 30,
        dependents: 0,
        income: 0,
        marital_status: 'single',
        risk_questions: [1, 0, 1],
        house: {
          ownership_status: 'owned',
        },
        vehicle: {
          year: new Date().getFullYear() + 1,
        },
      },
    ];

    const responses = await Promise.all(
      invalidRequest.map(async req => {
        const { status, body } = await request(app)
          .post('/assignment')
          .set('Content-Type', 'application/json')
          .set('Accept', 'application/json')
          .send(req);

        return {
          status,
          error_validation: !!body.validation,
        };
      }),
    );

    const getFalsyErros = responses.find(
      resp => resp.error_validation === false,
    );

    expect(getFalsyErros).toBeUndefined();
  });

  it('should be able to create an assignment', async () => {
    const { status, body } = await request(app)
      .post('/assignment')
      .set('Content-Type', 'application/json')
      .set('Accept', 'application/json')
      .send({
        age: 31,
        dependents: 3,
        house: {
          ownership_status: 'mortgaged',
        },
        income: 200000,
        marital_status: 'married',
        risk_questions: [0, 1, 0],
        vehicle: {
          year: 2020,
        },
      });

    expect(status).toBe(200);
    expect(body.auto).not.toBeUndefined();
    expect(body.auto).not.toBe('ineligible');
    expect(body.disability).not.toBeUndefined();
    expect(body.disability).not.toBe('ineligible');
    expect(body.home).not.toBeUndefined();
    expect(body.home).not.toBe('ineligible');
    expect(body.life).not.toBeUndefined();
    expect(body.life).not.toBe('ineligible');
  });
});
