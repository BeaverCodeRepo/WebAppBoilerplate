const { app } = require('@azure/functions');

app.http('Account', {
  methods: ['GET', 'POST'],
  handler: async (request, context) => {
    context.log('Http function processed request');

    const name = request.query.get('name') || (await request.text()) || 'world';

    return { body: `Hello, ${name}!` };
  },
});
